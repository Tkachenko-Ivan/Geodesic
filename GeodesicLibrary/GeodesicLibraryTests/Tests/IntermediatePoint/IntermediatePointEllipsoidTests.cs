﻿using GeodesicLibrary.Services;
using GeodesicLibraryTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeodesicLibraryTests.Tests.IntermediatePoint
{
    [TestClass]
    public class IntermediatePointEllipsoidTests : IntermediatePointTests
    {
        public sealed override IntermediatePointService IntermediatePointService { get; set; }

        public IntermediatePointEllipsoidTests()
        {
            IntermediatePointService = new IntermediatePointService(new Ellipsoid());
        }
    }
}