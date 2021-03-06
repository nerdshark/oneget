//
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

namespace Microsoft.PackageManagement.MetaProvider.PowerShell {
    using System;
    using System.Collections;
    using System.Linq;
    using Utility.Extensions;

    public class SoftwareIdentity : Yieldable {
        public SoftwareIdentity() {
        }

        public SoftwareIdentity(string fastPackageReference, string name, string version, string versionScheme, string source, string summary, string searchKey, string fullPath, string filename ) {
            FastPackageReference = fastPackageReference;
            Name = name;
            Version = version;
            VersionScheme = versionScheme;
            Source = source;
            Summary = summary;
            SearchKey = searchKey;
            FullPath = fullPath;
            Filename = filename;
        }

        public SoftwareIdentity(string fastPackageReference, string name, string version, string versionScheme, string source, string summary, string searchKey, string fullPath, string filename, Hashtable details, ArrayList entities, ArrayList links, bool fromTrustedSource)
            : this(fastPackageReference, name, version, versionScheme, source, summary, searchKey,fullPath, filename) {
            _details = details;
            _entities = entities;
            _links = links;
            FromTrustedSource = fromTrustedSource;
        }

        public SoftwareIdentity(string fastPackageReference, string name, string version, string versionScheme, string source, string summary, string searchKey, string fullPath, string filename, Hashtable details, ArrayList entities, ArrayList links, bool fromTrustedSource, ArrayList dependencies)
            : this(fastPackageReference, name, version, versionScheme, source, summary, searchKey, fullPath, filename) {
            _details = details;
            _entities = entities;
            _links = links;
            FromTrustedSource = fromTrustedSource;
            _dependencies = dependencies;
        }

        public string FastPackageReference {get; set;}
        public string Name {get; set;}
        public string Version {get; set;}
        public string VersionScheme {get; set;}
        public string Source {get; set;}
        public string Summary {get; set;}

        public string FullPath { get; set; }
        public string Filename { get; set; }

        public string SearchKey {get; set;}

        public bool FromTrustedSource {get; set;}

        public override bool YieldResult(PsRequest r) {
            if (r == null) {
                throw new ArgumentNullException("r");
            }

            return r.YieldSoftwareIdentity(FastPackageReference, Name, Version, VersionScheme, Summary, Source, SearchKey, FullPath, Filename) != null && YieldDetails(r) && YieldEntities(r) && YieldLinks(r) && YieldDependencies(r) && r.AddMetadata(FastPackageReference, "FromTrustedSource", FromTrustedSource.ToString()) != null;
        }

        private ArrayList _links;
        private ArrayList _entities;
        private ArrayList _dependencies;

        protected override bool YieldDetails(PsRequest r) {
            if (_details != null && _details.Count > 0) {
                // we need to send this back as a set of key/path & value  pairs.
                return _details.Flatten().All(kvp => r.AddMetadata(FastPackageReference,kvp.Key, kvp.Value) != null);
            }
            return true;
        }

        protected virtual bool YieldLinks(PsRequest r) {
            if( _links != null ) {
                return _links.OfType<Link>().All(link => r.AddLink(new Uri(link.HRef), link.Relationship, link.MediaType, link.Ownership, link.Use, link.AppliesToMedia, link.Artifact) != null);
            }
            return true;
        }

        protected virtual bool YieldDependencies(PsRequest r) {
            if (_dependencies != null) {
                return _dependencies.OfType<Dependency>().All(dep => r.AddDependency(dep.ProviderName, dep.PackageName, dep.Version, dep.Source, dep.AppliesTo ) != null);
            }
            return true;
        }


        protected virtual bool YieldEntities(PsRequest r) {
            if (_links != null) {
                return _entities.OfType<Entity>().All(entity => r.AddEntity(entity.Name, entity.RegId, entity.Role, entity.Thumbprint) != null);
            }
            return true;
        }
    }
}
