﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Gadgeteer Designer.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace ObjectTrackingDemo
{
    public partial class Program : Gadgeteer.Program
    {
        // GTM.Module defintions
		Gadgeteer.Modules.GHIElectronics.Extender PwmExtender;
		Gadgeteer.Modules.GHIElectronics.Extender AnalogExtender;
		Gadgeteer.Modules.GHIElectronics.Extender AnalogExtender2;
		Gadgeteer.Modules.GHIElectronics.Button button;

		public static void Main()
        {
			//Important to initialize the Mainboard first
            Mainboard = new GHIElectronics.Gadgeteer.FEZSpider();			

            Program program = new Program();
			program.InitializeModules();
            program.ProgramStarted();
            program.Run(); // Starts Dispatcher
        }

        private void InitializeModules()
        {   
			// Initialize GTM.Modules and event handlers here.		
			button = new GTM.GHIElectronics.Button(4);
		
			AnalogExtender = new GTM.GHIElectronics.Extender(9);
		
			AnalogExtender2 = new GTM.GHIElectronics.Extender(10);
		
			PwmExtender = new GTM.GHIElectronics.Extender(11);

        }
    }
}
