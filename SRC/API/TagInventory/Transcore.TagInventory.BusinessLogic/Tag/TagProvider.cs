using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcore.TagInventory.DataAccess;
using Transcore.TagInventory.Entity;
using Transcore.TagInventory.Entity.Common;
using Transcore.TagInventory.Entity.Core;
using Transcore.TagInventory.Entity.Model;

namespace Transcore.TagInventory.BusinessLogic
{
    public class TagProvider : ITagProvider
    {
        protected readonly ITagRepository _repository;
        public TagProvider(ITagRepository repository)
        {
            _repository = repository;
        }

        public Tag GetTag(long tagID)
        {
            return _repository.GetTag(tagID);
        }

        public Page<Tag> GetTags(TagSearch searchOptions, int pageSize, int pageNumber)
        {
            return _repository.GetTags(searchOptions, pageSize, pageNumber);
        }

        public void UpdateTagsStatus(List<Tag> tags)
        {
            _repository.UpdateTagsStatus(tags);
        }

        public List<TagActivityHistory> GetTagHistory(long tagID)
        {
            return _repository.GetTagHistory(tagID);
        }

        public void UpdateTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

             _repository.UpdateTag(tag);
        }

    }
}
