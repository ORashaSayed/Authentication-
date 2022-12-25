namespace JWT.API.Modules.Authentication
{
    public interface ITokenBlacklistService
    {
        void Add(string jti);
        void AddCurrentJti();
        bool IsBlackListed(string jti);
        bool IsCurrentJtiBlackListed();
    }
}