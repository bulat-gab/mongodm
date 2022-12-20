﻿//   Copyright 2020-present Etherna Sagl
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using Etherna.MongODM.Core.Serialization;

namespace Etherna.MongODM.Core.Options
{
    public class DocumentSemVerOptions
    {
        public SemanticVersion CurrentVersion { get; set; } = "1.0.0";
        public string ElementName { get; set; } = "_v";
        public bool WriteInDocuments { get; set; }
    }
}