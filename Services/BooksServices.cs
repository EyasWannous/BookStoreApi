using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace BookStoreApi.Services;

public class BooksServices
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksServices(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
          bookStoreDatabaseSettings.Value.ConnectionString
        );

        var mongoDatabase = mongoClient.GetDatabase(
          bookStoreDatabaseSettings.Value.DatabaseName
        );

        _booksCollection = mongoDatabase.GetCollection<Book>(
          bookStoreDatabaseSettings.Value.BooksCollectionName
        );
    }

    public async Task<List<Book>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task<ReplaceOneResult> UpdateAsync(string id, Book newBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, newBook);

    public async Task<DeleteResult> RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<long> CountAsync(Expression<Func<Book, bool>> filter, CountOptions? options = null, CancellationToken cancellationToken = default) =>
        await _booksCollection.CountDocumentsAsync(filter, options, cancellationToken);

    public async Task<DeleteResult> DeleteManyAsync(Expression<Func<Book, bool>> filter, DeleteOptions? options = null, CancellationToken cancellationToken = default) =>
        await _booksCollection.DeleteManyAsync(filter, options, cancellationToken);

    public async Task FindAsync(Expression<Func<Book, bool>> filter, FindOptions<Book, Book> options, CancellationToken cancellationToken = default) =>
        await _booksCollection.FindAsync(filter, options, cancellationToken);

    public async Task InsertManyAsync(IEnumerable<Book> documents, InsertManyOptions? options = null, CancellationToken cancellationToken = default) =>
        await _booksCollection.InsertManyAsync(documents, options, cancellationToken);

    public async Task UpdateManyAsync(Expression<Func<Book, bool>> filter, UpdateDefinition<Book> update, UpdateOptions? options = null, CancellationToken cancellationToken = default) =>
        await _booksCollection.UpdateManyAsync(filter, update, options, cancellationToken);

    public IMongoQueryable<Book> BookQueryable() => _booksCollection.AsQueryable();
}