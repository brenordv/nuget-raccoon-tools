using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using Raccoon.Ninja.Tools.OperationResult;
using Raccoon.Ninja.Tools.OperationResult.ResultError;

namespace Raccoon.Ninja.Tools.Benchmark.Tests.Benchmarks;

[MemoryDiagnoser, RankColumn]
[ExcludeFromCodeCoverage]
public class OperationResultBenchmark
{
    private static readonly BenchMarkOperations Operations = new ();
    
    [Benchmark(Baseline = true)]
    public int ThrowExceptionOnError()
    {
        try
        {
            return Operations.ThrowExceptionOnError();
        }
        catch (Exception e)
        {
            return -1;
        }
    }
    
    [Benchmark]
    public Result<int> ReturnResultOnError()
    {
        return Operations.ReturnResultOnError();
    }
    
    [Benchmark]
    public string ReturnNullOnExceptionCaught()
    {
        return Operations.ReturnNullOnExceptionCaught();
    }
    
    [Benchmark]
    public Result<string> ReturnImplicitResultOnExceptionCaught()
    {
        return Operations.ReturnImplicitResultOnExceptionCaught();
    }
    
    [Benchmark]
    public Result<string> ReturnExplicitResultOnExceptionCaught()
    {
        return Operations.ReturnExplicitResultOnExceptionCaught();
    }
    
    [Benchmark]
    public IList<string> ReturnSuccessList()
    {
        return Operations.ReturnSuccessList();
    }
    
    [Benchmark]
    public Result<IList<string>> ReturnSuccessResultList()
    {
        return Operations.ReturnSuccessResultList();
    }

    private sealed class BenchMarkOperations
    {
        public int ThrowExceptionOnError()
        {
            //Something goes wrong, and then you...
            throw new Exception("Something went wrong. That makes me sad, but it's excepted.");
        }
        
        public Result<int> ReturnResultOnError()
        {
            //Something goes wrong, and then you...
            return new Result<int>(new Error("Something went wrong. That makes me sad, but it's excepted."));
        }

        public string ReturnNullOnExceptionCaught()
        {
            try
            {
                throw new Exception("Well, this is unexpected...");

            }
            catch (Exception e)
            {
                //You might want to log something...
                return null;
            }
        }

        public Result<string> ReturnImplicitResultOnExceptionCaught()
        {
            try
            {
                throw new Exception("Well, this is unexpected...");

            }
            catch (Exception e)
            {
                //You might want to log something...
                
                //Exception is implicitly converted to an Error Result<string>.
                return e;
            }
        }
        
        public Result<string> ReturnExplicitResultOnExceptionCaught()
        {
            try
            {
                throw new Exception("Well, this is unexpected...");

            }
            catch (Exception e)
            {
                //You might want to log something...

                return new Result<string>(new Error("Don't worry, I got this.", e));
            }
        }

        public IList<string> ReturnSuccessList()
        {
            return new List<string> {"One", "Two", "Three"};
        }
        
        public Result<IList<string>> ReturnSuccessResultList()
        {
            return new List<string> {"One", "Two", "Three"};
        }
    }
}