namespace Forums.Core
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ProjectContext _context;

        public UnitOfWork(ProjectContext context)
        {
            _context = context;
        }

        void IUnitOfWork.Save()
        {
            _context.SaveChanges();
        }
    }
}
