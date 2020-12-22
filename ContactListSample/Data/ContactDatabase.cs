using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactListSample.extensions;
using ContactListSample.Models;
using SQLite;

namespace ContactListSample.Data
{
    public class ContactDatabase
    {

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public ContactDatabase()
        {

            InitializeAsync().SafeFireAndForget(false);

        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Contact).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Contact)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<Contact>> GetContactsAsync()
        {
            return Database.Table<Contact>().ToListAsync();
        }

    }
}
