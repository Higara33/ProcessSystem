using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DB;
using Microsoft.EntityFrameworkCore;
using ProcessSystem.Contracts;

namespace ProcessSystem.DB
{
    public class RegisterRepository
        : IRegisterRepository
    {
        public readonly ProcessContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public RegisterRepository(ProcessContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Register> AddAsync(Register register)
        {

            return (await _context.Register
                    .AddAsync(register))
                .Entity;
        }

        public Register FindByNameAndUrl(Register register)
        {
            return new List<Register>
            {
                new Register("fhasdbvdhfb", "www", "user"),
                new Register("afbdvSDVw", "rrr", "user2"),
                new Register("hgfdasd", "ttt", "user3")
            }
            .SingleOrDefault(sr => sr.Name == register.Name && sr.Url == register.Url);
        }

        public async Task<Register> FindByTokenAsync(string token)
        {

            var list = new List<Register> 
            {
                new Register("fhasdbvdhfb", "www", "user"),
                new Register("afbdvSDVw", "rrr", "user2"),
                new Register("hgfdasd", "ttt", "user3")
            };
                return list.SingleOrDefault(sr => sr.Token == token);

        }

        public async Task<Register> DeleteAsync(string token)
        {
            var swr =await FindByTokenAsync(token);

            if (swr is null) throw new ArgumentNullException(nameof(swr), "По токену не найдена запись");

            _context.Register.Remove(swr);

            return swr;
        }
    }
}
