namespace YssWebstoreApi.Middlewares.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AllowUnverifiedAttribute : Attribute { }
}
