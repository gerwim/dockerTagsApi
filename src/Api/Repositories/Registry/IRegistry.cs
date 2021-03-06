﻿using System.Threading.Tasks;
using Api.Models;

namespace Api.Repositories.Registry
{
    public interface IRegistry
    {
        public string FriendlyUrl  { get; }
        Task<TagsReponseModel> ListTags(string imageName, string searchForRegex);
    }
}