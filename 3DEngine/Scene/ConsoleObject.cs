using System;
using System.Drawing;

namespace _3DEngine
{
    public class ConsoleObject : SceneObject
    {
        public double SecondDisplayDiagonal { get; set; } = 55;
        public double ManipulatorRadius { get; set; } = 3;
        public double ManipulatorBaseRadius { get; set; } = 5;
        public double CylindersHeight { get; set; } = 2.5;
        public double CylindersRadius { get; set; } = 2.5;
        public double CrossButtonSize { get; set; } = 5;
        public double VolumeSpace { get; set; } = 1;
        public double FrameThickness { get; set; } = 1;
        public double BrightnessBottomPosition { get; set; } = 15;
        public double CardReaderWidth { get; set; } = 15;
        public int CardReadersCount { get; set; } = 1;
        public int IndicatorsCount { get; set; } = 3;
        
        public ConsoleObject(string name) : base(name)
        {
            UpdateObject();
        }

        public void UpdateObject()
        {
            ScenePrimitives.Clear();

            ScenePrimitive bottom = new ScenePrimitive(
                new Box(new Point3D(0, 0, 0), 90, 5, 60, Color.FromArgb(85, 85, 85)),
                "Bottom"
                );
            ScenePrimitives.Add(bottom);

            ScenePrimitive leftRack = new ScenePrimitive(
                new Box(new Point3D(-42.5, 5, 27.5), 5, 5, 5, Color.FromArgb(85, 85, 85)),
                "Left Rack"
                );
            ScenePrimitives.Add(leftRack);

            ScenePrimitive rightRack = new ScenePrimitive(
                new Box(new Point3D(42.5, 5, 27.5), 5, 5, 5, Color.FromArgb(85, 85, 85)),
                "Right Rack"
                );
            ScenePrimitives.Add(rightRack);

            ScenePrimitive underframe = new ScenePrimitive(
                new Cylinder(new Point3D(-40, 7.5, 27.5), 2.5, 80, 16, Color.FromArgb(75, 75, 75)),
                "Underframe"
                );
            underframe.AngleZ = 90;
            ScenePrimitives.Add(underframe);

            ScenePrimitive holder = new ScenePrimitive(
                new Box(new Point3D(0, 10, 27.5), 80, 2.5, 1, Color.FromArgb(55, 55, 55)),
                "Holder"
                );
            ScenePrimitives.Add(holder);

            double displayThickness = 5 - FrameThickness;
            double displayDepth = 30 - displayThickness / 2;
            ScenePrimitive display = new ScenePrimitive(
                new Box(new Point3D(0, 12.5, displayDepth), 90, 55, displayThickness, Color.FromArgb(100, 100, 100)),
                "Screen"
                );
            ScenePrimitives.Add(display);

            double frameDepth = 30 - displayThickness - FrameThickness / 2;
            ScenePrimitive leftFrame = new ScenePrimitive(
                new Box(new Point3D(-42.5, 12.5, frameDepth), 5, 55, FrameThickness, Color.FromArgb(75, 75, 75)),
                "Left Frame"
                );
            ScenePrimitives.Add(leftFrame);

            ScenePrimitive rightFrame = new ScenePrimitive(
                new Box(new Point3D(42.5, 12.5, frameDepth), 5, 55, FrameThickness, Color.FromArgb(75, 75, 75)),
                "Right Frame"
                );
            ScenePrimitives.Add(rightFrame);

            ScenePrimitive topFrame = new ScenePrimitive(
                new Box(new Point3D(0, 62.5, frameDepth), 80, 5, FrameThickness, Color.FromArgb(75, 75, 75)),
                "Top Frame"
                );
            ScenePrimitives.Add(topFrame);

            ScenePrimitive bottomFrame = new ScenePrimitive(
                new Box(new Point3D(0, 12.5, frameDepth), 80, 5, FrameThickness, Color.FromArgb(75, 75, 75)),
                "Bottom Frame"
                );
            ScenePrimitives.Add(bottomFrame);

            double volumeHeight = 5;
            double displayTop = display.Primitive.BasePoint.Y + ((Box)display.Primitive).Height;
            double volumeBottom = displayTop - volumeHeight - 10;
            ScenePrimitive volumeUp = new ScenePrimitive(
                new Box(new Point3D(-45.5, volumeBottom, 28.5), 1, 5, 1, Color.FromArgb(155, 155, 155)),
                "Volume Up"
                );
            ScenePrimitives.Add(volumeUp);

            volumeBottom = volumeBottom - (volumeHeight + VolumeSpace);
            ScenePrimitive volumeDown = new ScenePrimitive(
                new Box(new Point3D(-45.5, volumeBottom, 28.5), 1, 5, 1, Color.FromArgb(155, 155, 155)),
                "Volume Down"
                );
            ScenePrimitives.Add(volumeDown);

            double displayBottom = display.Primitive.BasePoint.Y;
            double brightnessBottom = displayBottom + BrightnessBottomPosition;
            ScenePrimitive brightness = new ScenePrimitive(
                new Box(new Point3D(-45.5, brightnessBottom, 28.5), 1, 10, 1, Color.FromArgb(155, 155, 155)),
                "Brightness"
                );
            ScenePrimitives.Add(brightness);

            double secondDisplayWidth = Math.Sqrt(Math.Pow(SecondDisplayDiagonal, 2) / 2);
            ScenePrimitive secondDisplay = new ScenePrimitive(
                new Box(new Point3D(0, 5, -2.5), secondDisplayWidth, 1, secondDisplayWidth, Color.FromArgb(75, 75, 75)),
                "Second Screen"
                );
            ScenePrimitives.Add(secondDisplay);

            ScenePrimitive cylinderUp = new ScenePrimitive(
                new Cylinder(new Point3D(32.5, 5, -10), CylindersRadius, CylindersHeight, 16, Color.FromArgb(155, 155, 155)),
                "Cylinder Up"
                );
            ScenePrimitives.Add(cylinderUp);

            ScenePrimitive cylinderDown = new ScenePrimitive(
                new Cylinder(new Point3D(32.5, 5, 0), CylindersRadius, CylindersHeight, 16, Color.FromArgb(155, 155, 155)),
                "Cylinder Down"
                );
            ScenePrimitives.Add(cylinderDown);

            ScenePrimitive cylinderLeft = new ScenePrimitive(
                new Cylinder(new Point3D(27.5, 5, -5), CylindersRadius, CylindersHeight, 16, Color.FromArgb(155, 155, 155)),
                "Cylinder Left"
                );
            ScenePrimitives.Add(cylinderLeft);

            ScenePrimitive cylinderRight = new ScenePrimitive(
                new Cylinder(new Point3D(37.5, 5, -5), CylindersRadius, CylindersHeight, 16, Color.FromArgb(155, 155, 155)),
                "Cylinder Right"
                );
            ScenePrimitives.Add(cylinderRight);

            ScenePrimitive crossMiddle = new ScenePrimitive(
                new Box(new Point3D(-32.5, 5, 10), CrossButtonSize * 3, 2, CrossButtonSize, Color.FromArgb(155, 155, 155)),
                "Cross Middle"
                );
            ScenePrimitives.Add(crossMiddle);

            ScenePrimitive crossTop = new ScenePrimitive(
                new Box(new Point3D(-32.5, 5, 10 + CrossButtonSize), CrossButtonSize, 2, CrossButtonSize, crossMiddle.Primitive.Color),
                "Cross Top"
                );
            ScenePrimitives.Add(crossTop);

            ScenePrimitive crossBottom = new ScenePrimitive(
                new Box(new Point3D(-32.5, 5, 10 - CrossButtonSize), CrossButtonSize, 2, CrossButtonSize, crossMiddle.Primitive.Color),
                "Cross Bottom"
                );
            ScenePrimitives.Add(crossBottom);

            ScenePrimitive hemisphereBase = new ScenePrimitive(
                new Cylinder(new Point3D(-32.5, 5, -15), ManipulatorBaseRadius, 1, 16, Color.FromArgb(155, 155, 155)),
                "Hemisphere Base"
                );
            ScenePrimitives.Add(hemisphereBase);

            ScenePrimitive hemisphere = new ScenePrimitive(
                new Hemisphere(new Point3D(-32.5, 6, -15), ManipulatorRadius, 24, Color.FromArgb(100, 100, 100), false),
                "Hemisphere"
                );
            ScenePrimitives.Add(hemisphere);

            double readerLeft = -35;
            for (int i = 0; i < CardReadersCount; ++i)
            {
                ScenePrimitive reader = new ScenePrimitive(
                    new Box(new Point3D(readerLeft + CardReaderWidth / 2, 1, -30.5), CardReaderWidth, 3, 1, Color.FromArgb(155, 155, 155)),
                    $"Reader-{i}"
                );
                ScenePrimitives.Add(reader);
                readerLeft += CardReaderWidth + 5;
            }

            double indicatorLeft = -(IndicatorsCount + (IndicatorsCount - 1) * 3) / 2;
            for (int i = 0; i < IndicatorsCount; ++i)
            {
                ScenePrimitive indicator = new ScenePrimitive(
                    new Box(new Point3D(indicatorLeft + 0.5, 5, -28.5), 1, 1, 3, Color.FromArgb(55, 55, 55)),
                    $"Indicator-{i}"
                    );
                ScenePrimitives.Add(indicator);
                indicatorLeft += 4;
            }
        }
    }
}
