using ELibrary.Data;

namespace ELibrary.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ELibraryContext _context;

        private IEmployeeRepository _employeeRepository;
        private IMemberRepository _memberRepository;
        private IPhoneRepository _phoneRepository;
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        private IBookAuthorRepository _bookAuthorRepository;
        private IBorrowingRepository _borrowingRepository;

        public UnitOfWork(ELibraryContext context)
        {
            _context = context;
        }

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (_employeeRepository == null)
                {
                    _employeeRepository = new EmployeeRepository(_context);
                }

                return _employeeRepository;
            }
        }

        public IMemberRepository MemberRepository
        {
            get
            {
                if (_memberRepository == null)
                {
                    _memberRepository = new MemberRepository(_context);
                }

                return _memberRepository;
            }
        }

        public IPhoneRepository PhoneRepository
        {
            get
            {
                if (_phoneRepository == null)
                {
                    _phoneRepository = new PhoneRepository(_context);
                }

                return _phoneRepository;
            }
        }
        
        public IAuthorRepository AuthorRepository
        {
            get
            {
                if (_authorRepository == null)
                {
                    _authorRepository = new AuthorRepository(_context);
                }

                return _authorRepository;
            }
        }

        public IBookRepository BookRepository
        {
            get
            {
                if (_bookRepository == null)
                {
                    _bookRepository = new BookRepository(_context);
                }

                return _bookRepository;
            }
        }
        
        public IBookAuthorRepository BookAuthorRepository
        {
            get
            {
                if (_bookAuthorRepository == null)
                {
                    _bookAuthorRepository = new BookAuthorRepository(_context);
                }

                return _bookAuthorRepository;
            }
        }

        public IBorrowingRepository BorrowingRepository
        {
            get
            {
                if (_borrowingRepository == null)
                {
                    _borrowingRepository = new BorrowingRepository(_context);
                }

                return _borrowingRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}