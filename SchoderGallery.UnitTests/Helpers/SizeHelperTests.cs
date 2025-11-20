using SchoderGallery.DTOs;
using SchoderGallery.Helpers;

namespace SchoderGallery.UnitTests.Helpers;

//[TestClass]
//public sealed class SizeHelperTests
//{
//    [DataTestMethod]
//    [DataRow(SizeType.Fixed, 2000, 2000, 1920, 1080, 1080, 1080)] // artwork w, h larger than screen 
//    [DataRow(SizeType.Fixed, 2000, 1000, 1920, 1080, 1920, 960)] // artwork w larger than screen 
//    [DataRow(SizeType.Fixed, 1920, 1920, 1920, 1080, 1080, 1080)] // artwork h larger than screen 
//    [DataRow(SizeType.Fixed, 1919, 1079, 1920, 1080, 1919, 1079)] // artwork w,h smaller than screen 
//    [DataRow(SizeType.Ratio, 1, 1, 1920, 1080, 1080, 1080)]
//    [DataRow(SizeType.Ratio, 2, 1, 1920, 1080, 1920, 960)]
//    [DataRow(SizeType.Ratio, 1, 2, 1920, 1080, 540, 1080)]
//    [DataRow(SizeType.Ratio, 16, 9, 1920, 1080, 1920, 1080)]
//    [DataRow(SizeType.Dynamic, 0, 0, 1921, 1080, 1920, 1080)] // artwork w,h don't matter, stretch to the max, but not more than 16 out of 16:9
//    [DataRow(SizeType.Dynamic, 0, 0, 1080, 1921, 1080, 1920)] // artwork w,h don't matter, stretch to the max, but not more than 9 out of 16:9
//    [DataRow(SizeType.Dynamic, 0, 0, 1900, 1080, 1900, 1080)] // artwork w,h don't matter, stretch to the max, but not more than 9 out of 16:9
//    public void GetArtworkSize_VariousSizes_ReturnsExpectedResult(
//        SizeType sizeType,
//        int artworkWidth,
//        int artworkHeight,
//        int screenWidth,
//        int screenHeight,
//        int expectedWidth,
//        int expectedHeight)
//    {
//        // Arrange
//        var factory = new SizeHelperFactory(
//        [
//            new FixedSizeHelper(),
//            new RatioSizeHelper(),
//            new DynamicSizeHelper()
//        ]);

//        var sizeHelper = factory.GetHelper(sizeType); ;

//        var artwork = new ArtworkDto(
//            title: string.Empty,
//            year: 0,
//            renderAlgorithm: null,
//            sizeType: sizeType,
//            width: artworkWidth,
//            height: artworkHeight,
//            artist: string.Empty,
//            id: 0
//        );

//        // Act
//        var (width, height) = sizeHelper.GetArtworkSize(artwork, screenWidth, screenHeight);

//        // Assert
//        Assert.AreEqual(expectedWidth, width, $"Expected width {expectedWidth} but got {width}.");
//        Assert.AreEqual(expectedHeight, height, $"Expected height {expectedHeight} but got {height}.");
//    }
//}