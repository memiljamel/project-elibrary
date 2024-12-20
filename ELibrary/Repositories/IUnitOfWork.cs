﻿namespace ELibrary.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IStaffRepository StaffRepository { get; }

        IMemberRepository MemberRepository { get; }

        IPhoneRepository PhoneRepository { get; }

        IAuthorRepository AuthorRepository { get; }

        IBookRepository BookRepository { get; }

        IBookAuthorRepository BookAuthorRepository { get; }

        IBorrowingRepository BorrowingRepository { get; }

        Task SaveChangesAsync();
    }
}
