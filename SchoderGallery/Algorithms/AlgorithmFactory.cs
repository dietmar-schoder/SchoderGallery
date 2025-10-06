namespace SchoderGallery.Algorithms;

public class AlgorithmFactory(IEnumerable<IAlgorithm> algorithmList)
{
    private readonly Dictionary<AlgorithmType, IAlgorithm> _algorithmList = algorithmList.ToDictionary(a => a.AlgorithmType, a => a);

    public IAlgorithm GetAlgorithm(AlgorithmType algorithmType) =>
        _algorithmList[algorithmType];
}