using System.Runtime.Caching;
using CSRunner;

namespace CSRunnerTests;

public class VerifyTests
{
    private List<string> _validPrograms = new (), _invalidPrograms = new ();
    private Evaluator _evaluator;
    [SetUp]
    public void Setup()
    {
        _validPrograms = Directory.EnumerateFiles("./Lambdas/Valid").Select(File.ReadAllText).ToList();
        _invalidPrograms = Directory.EnumerateFiles("Lambdas/Invalid").Select(File.ReadAllText).ToList();
        _evaluator = new Evaluator(new MemoryCache("big cache"), new CacheItemPolicy(), new RandomDataFactory(new Random()));
    }
    
    [Test]
    public void TestValidPrograms()
    {
        foreach (var program in _validPrograms)
        {
            Assert.That(_evaluator.Verify(program), Is.True);
        }
    }

    [Test]
    public void TestInvalidPrograms()
    {
        foreach (var program in _invalidPrograms)
        {
            Assert.That(_evaluator.Verify(program), Is.False);
        }
    }
}