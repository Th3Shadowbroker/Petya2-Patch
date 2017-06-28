using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petya_2_Patcher.Classes
{
    class Patch
    {

        /// <summary>
        /// The key we want to patch.
        /// </summary>
        private RegistryKey RootKey = Registry.LocalMachine.OpenSubKey( "SYSTEM\\CurrentControlSet\\Services\\LanmanServer\\Parameters", true );

        /// <summary>
        /// Check patch state.
        /// </summary>
        /// <returns>bool</returns>
        public bool isPatched()
        {
            return RootKey.GetValue( "SMB1" ) != null;
        }

        /// <summary>
        /// Patch root key.
        /// </summary>
        public void patch()
        {
            RootKey.SetValue( "SMB1", 0, RegistryValueKind.DWord );
        }

        /// <summary>
        /// Remove the patch from registry.
        /// </summary>
        public void unpatch()
        {
            RootKey.DeleteValue( "SMB1" );
        }

    }
}
