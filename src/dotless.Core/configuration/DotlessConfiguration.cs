/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

namespace dotless.Core.configuration
{
    using System;

    public class DotlessConfiguration
    {
        public DotlessConfiguration()
        {
            LessSource = typeof(FileSource);
        }

        public bool MinifyOutput { get; set; }
        public bool CacheEnabled { get; set; }
        public Type LessSource { get; set; }
        public static DotlessConfiguration DefaultWeb
        {
            get
            {
                var defaultConfiguration = Default;
                defaultConfiguration.LessSource = typeof (AspServerPathSource);
                return defaultConfiguration;
            }
        }
        public static DotlessConfiguration Default
        {
            get
            {
                return new DotlessConfiguration
                           {
                               LessSource = typeof (FileSource),
                               MinifyOutput = false,
                               CacheEnabled = true
                           };
            }
        }
    }
}