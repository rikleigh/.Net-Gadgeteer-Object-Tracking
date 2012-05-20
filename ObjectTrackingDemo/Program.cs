using System;
using System.Threading;
using Microsoft.SPOT;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using GadgeteerObjectTracking;

namespace ObjectTrackingDemo
{
    public partial class Program
    {
        ObjectTracker _objectTracker;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            Setup();

            button.ButtonPressed += new Button.ButtonEventHandler(button_ButtonPressed);
        }

        void button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            if (_objectTracker == null)
            {
                throw new ApplicationException("object tracker is null");
            }

            if (_objectTracker.IsTracking)
            {
                _objectTracker.StopTracking();
                button.TurnLEDOff();
            }
            else
            {
                _objectTracker.StartTracking();
                button.TurnLEDOn();
            }
        }

        void Setup()
        {
            var irEye = new IrCompoundEye(AnalogExtender, AnalogExtender2);
            var panAndTiltController = new PanAndTiltController(PwmExtender, 20000000);
            _objectTracker = new ObjectTracker(irEye, panAndTiltController);
        }
    }
}
