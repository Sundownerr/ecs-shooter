using Scellecs.Morpeh;

namespace Game
{
    public static class FilterExtentions
    {
        public static FilterBuilder With<T1, T2>(this FilterBuilder builder)
            where T1 : struct, IComponent
            where T2 : struct, IComponent =>
            builder.With<T1>().With<T2>();

        public static FilterBuilder With<T1, T2, T3>(this FilterBuilder builder)
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent =>
            builder.With<T1>().With<T2>().With<T3>();

        public static FilterBuilder With<T1, T2, T3, T4>(this FilterBuilder builder)
            where T1 : struct, IComponent
            where T2 : struct, IComponent
            where T3 : struct, IComponent
            where T4 : struct, IComponent =>
            builder.With<T1>().With<T2>().With<T3>().With<T4>();
    }
}