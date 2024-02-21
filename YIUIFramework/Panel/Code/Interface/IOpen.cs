using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public interface IOpen
    {
    }

    public interface IOpen<in P1> : IOpen
    {
        UniTask<bool> OnOpen(P1 p1);
    }

    public interface IOpen<in P1, in P2> : IOpen
    {
        UniTask<bool> OnOpen(P1 p1, P2 p2);
    }

    public interface IOpen<in P1, in P2, in P3> : IOpen
    {
        UniTask<bool> OnOpen(P1 p1, P2 p2, P3 p3);
    }

    public interface IOpen<in P1, in P2, in P3, in P4> : IOpen
    {
        UniTask<bool> OnOpen(P1 p1, P2 p2, P3 p3, P4 p4);
    }

    public interface IOpen<in P1, in P2, in P3, in P4, in P5> : IOpen
    {
        UniTask<bool> OnOpen(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    }
}