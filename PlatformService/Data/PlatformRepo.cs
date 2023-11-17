using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;

        #region CTOR

        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        public void CreatePlatform(Platform platform)
        {
            
            if(platform is null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _context.Platforms.Add(platform);

        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms ?? Enumerable.Empty<Platform>();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}