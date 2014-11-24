﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using XBee.Frames;
using XBee.Frames.AtCommands;

namespace XBee.Test
{
    [TestClass]
    public class FrameTest
    {
        private readonly FrameSerializer _frameSerializer = new FrameSerializer();

        private void Check(FrameContent frameContent, byte[] expectedValue)
        {
            var frame = new Frame(frameContent);
            var actualValue = _frameSerializer.Serialize(frame);

            Assert.AreEqual(expectedValue.Length, actualValue.Length, "Actual data length does not match expected length.");

            for (int i = 0; i < expectedValue.Length; i++)
            {
                var expected = expectedValue[i];
                var actual = actualValue[i];

                Assert.AreEqual(expected, actual,
                    string.Format("Value at position {0} does not match expected value.", i));
            }
        }

        [TestMethod]
        public void AtCommandFrameTest()
        {
            var atCommandFrame = new AtCommandFrameContent("NH") {FrameId = 0x52};

            var expectedValue = new byte[] { 0x7e, 0x00, 0x04, 0x08, 0x52, 0x4e, 0x48, 0x0f };

            Check(atCommandFrame, expectedValue);
        }

        [TestMethod]
        public void AtCommandResponseFrameTest()
        {
            var atResponseCommandFrame = new AtCommandResponseFrame {AtCommand = "BD", FrameId = 0x01};

            var expectedValue = new byte[] { 0x7e, 0x00, 0x05, 0x88, 0x01, 0x42, 0x44, 0x00, 0xf0 };

            Check(atResponseCommandFrame, expectedValue);
        }

        [TestMethod]
        public void TxRequestFrameTest()
        {
            var txRequestFrame = new TxRequestExtFrame(new LongAddress(0x0013A200400A0127),
                new byte[] {0x54, 0x78, 0x44, 0x61, 0x74, 0x61, 0x30, 0x41}) {FrameId = 0x01};

            var expectedValue = new byte[]
            {
                0x7e, 0x00, 0x16, 0x10, 0x01, 0x00, 0x13, 0xA2, 
                0x00, 0x40, 0x0A, 0x01, 0x27, 0xff, 0xfe, 0x00, 
                0x00, 0x54, 0x78, 0x44, 0x61, 0x74, 0x61, 0x30, 
                0x41, 0x13
            };

            Check(txRequestFrame, expectedValue);
        }

        [TestMethod]
        public void TxStatusExtFrameTest()
        {
            var txStatusFrame = new TxStatusExtFrame
            {
                FrameId = 0x47,
                DiscoveryStatus = DiscoveryStatus.RouteDiscovery
            };

            var expectedValue = new byte[] {0x7e, 0x00, 0x07, 0x8b, 0x47, 0xff, 0xfe, 0x00, 0x00, 0x02, 0x2e};

            Check(txStatusFrame, expectedValue);
        }

        [TestMethod]
        public void AtCommand_CoordinatorEnable_FrameTest()
        {
            var atCommandFrame = new CoordinatorEnableCommand { FrameId = 0x33 };

            var expectedValue = new byte[] { 0x7e, 0x00, 0x04, 0x08, 0x33, 0x43, 0x45, 0x3c };

            Check(atCommandFrame, expectedValue);
        }


        [TestMethod]
        public void AtCommand_CoordinatorEnableWithParam_FrameTest()
        {
            var atCommandFrame = new CoordinatorEnableCommand(true) { FrameId = 0x33 };

            var expectedValue = new byte[] { 0x7e, 0x00, 0x05, 0x08, 0x33, 0x43, 0x45, 0x1, 0x3b };

            Check(atCommandFrame, expectedValue);
        }
    }
}