using System;
using System.Threading;
using GT = Gadgeteer;
using Gadgeteer.Modules.GHIElectronics;

namespace GadgeteerObjectTracking
{
    public class IrCompoundEye
    {
        public GT.Interfaces.AnalogInput AnalogLeft { get; set; }

        public GT.Interfaces.AnalogInput AnalogRight { get; set; }

        public GT.Interfaces.AnalogInput AnalogUp { get; set; }

        public GT.Interfaces.AnalogInput AnalogDown { get; set; }

        public GT.Interfaces.DigitalOutput IrLeds { get; set; }

        public IrCompoundEye(Extender extender1, Extender extender2)
        {
            if (extender1 == null || extender2 == null)
            {
                throw new ApplicationException("analog extender modules not set up correctly");
            }

            AnalogLeft = extender1.SetupAnalogInput(GT.Socket.Pin.Three);
            AnalogRight = extender1.SetupAnalogInput(GT.Socket.Pin.Four);
            AnalogDown = extender2.SetupAnalogInput(GT.Socket.Pin.Three);
            AnalogUp = extender2.SetupAnalogInput(GT.Socket.Pin.Four);           
            IrLeds = extender1.SetupDigitalOutput(GT.Socket.Pin.Six, false);      
        }

        public IrCompoundEyeData Read()
        {
            var multiplier = 310;
            // Total IR = Ambient IR + LED IR Rreflected from object

            // turn on IR LEDs to read TOTAL IR LIGHT (ambient + reflected)
            IrLeds.Write(true);
            // Allow time for phototransistors to respond. (may not be needed)                   
            Thread.Sleep(1);

            // read sensors  
            var leftIrValue = AnalogLeft.ReadVoltage();
            var rightIrValue = AnalogRight.ReadVoltage();
            var upIrValue = AnalogUp.ReadVoltage();
            var downIrValue = AnalogDown.ReadVoltage();

            // turn off IR LEDs to read Ambient IR Light (IR from indoor lighting and sunlight)
            IrLeds.Write(false);
            // Allow time for phototransistors to respond. (may not be needed)                  
            Thread.Sleep(1);

            // Reflected IR = Total IR - Ambient IR
            // read sensors again and subtract this value from our first reading
            leftIrValue = (leftIrValue - AnalogLeft.ReadVoltage()) * multiplier;
            rightIrValue = (rightIrValue - AnalogRight.ReadVoltage()) * multiplier;
            upIrValue = (upIrValue - AnalogUp.ReadVoltage()) * multiplier;
            downIrValue = (downIrValue - AnalogDown.ReadVoltage()) * multiplier;

            return new IrCompoundEyeData
            {
                LeftIrValue = leftIrValue,
                RightIrValue = rightIrValue,
                UpIrValue = upIrValue,
                DownIrValue = downIrValue,
                CurrentDistance = GetDistance(leftIrValue, rightIrValue, upIrValue, downIrValue)
            };
        }

        private double GetDistance(double left, double right, double up, double down)
        {
            // distance of object is average of reflected IR
            return (left + right + up + down) / 4;
        }
    }
}
