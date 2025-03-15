//Agenda03- DSI 3- VANDETH CAETANO

using MauiAppMinhasCompras01.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras01.Helpers
{
    public class SQLiteDatabaseHelper
    {
        private readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }


        public async Task<int> Insert(Produto p)
        {
            return await _conn.InsertAsync(p);
        }

        public async Task<int> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return await _conn.ExecuteAsync(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
        }


        public async Task<int> Delete(int id)
        {
            return await _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

 
        public async Task<List<Produto>> GetAll()
        {
            return await _conn.Table<Produto>().ToListAsync();
        }

     
        public async Task<List<Produto>> Search(string q)
        {
            return await _conn.QueryAsync<Produto>("SELECT * FROM Produto WHERE Descricao LIKE ?", "%" + q + "%");
        }
    }
}
