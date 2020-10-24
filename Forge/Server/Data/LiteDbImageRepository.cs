using Forge.Shared.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public class LiteDbImageRepository: IDbImageRepository
    {
        private LiteDatabase _liteDb;

        public LiteDbImageRepository(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<ImageModel> FindAll(bool includeDeleted = false)
        {
            var fs = _liteDb.GetStorage<Guid>("imageFiles", "imageChunks");
            var models = fs.FindAll()
                .Select(file => {
                    var model = new ImageModel() { 
                        Id = file.Metadata["_id"],
                        Title = file.Metadata["title"].AsString,
                        Body = file.Metadata["body"].AsString,
                        Filename = file.Metadata["filename"].AsString,
                        Type = file.Metadata["type"].AsString,
                        Uploaded = file.Metadata["uploaded"],
                        Updated = file.Metadata["updated"],
                        Deleted = file.Metadata["deleted"],
                    };

                    return model;
                })
                .Where(model => model.Deleted == false || includeDeleted);

            return models;
        }

        public ImageModel FindOne(Guid id, bool includeDeleted = false)
        {
            var fs = _liteDb.GetStorage<Guid>("imageFiles", "imageChunks");
            var file = fs.FindById(id);

            var model = new ImageModel()
            {
                Id = file.Metadata["_id"],
                Title = file.Metadata["title"].AsString,
                Body = file.Metadata["body"].AsString,
                Filename = file.Metadata["filename"].AsString,
                Type = file.Metadata["type"].AsString,
                Uploaded = file.Metadata["uploaded"],
                Updated = file.Metadata["updated"],
                Deleted = file.Metadata["deleted"],
            };

            if (model.Deleted == false || includeDeleted)
                return model;
            else
                return null;
        }

        public IEnumerable<ImageModel> FindRange(Guid[] ids, bool includeDeleted = false)
        {
            var all = FindAll(includeDeleted);
            return all.Where(model => ids.Contains(model.Id));
        }

        public Stream GetContent(Guid id)
        {
            var fs = _liteDb.GetStorage<Guid>("imageFiles", "imageChunks");
            var file = fs.FindById(id);

            return file.OpenRead();
        }

        public Guid Insert(ImageModel model)
        {
            throw new NotImplementedException();
        }

        public Guid Insert(ImageModel model, string contentBase64)
        {
            var fs = _liteDb.GetStorage<Guid>("imageFiles", "imageChunks");

            // Base64 -> Stream
            var bytes = Convert.FromBase64String(contentBase64);
            var contents = new MemoryStream(bytes);

            var metadata = new BsonDocument();
            metadata["_id"] = model.Id;
            metadata["title"] = model.Title;
            metadata["body"] = model.Body;
            metadata["filename"] = model.Filename;
            metadata["type"] = model.Type;
            metadata["uploaded"] = DateTime.Now;
            metadata["updated"] = DateTime.Now;
            metadata["deleted"] = model.Deleted;
            fs.Upload(model.Id, model.Filename, contents, metadata);

            return model.Id;
        }

        public bool Update(ImageModel model)
        {
            return Update(model, null);
        }

        public bool Update(ImageModel model, string contentBase64 = null)
        {
            var fs = _liteDb.GetStorage<Guid>("imageFiles", "imageChunks");

            var existing = fs.FindById(model.Id);
            if (existing != null)
            {
                if (string.IsNullOrEmpty(contentBase64))
                {
                    var metadata = new BsonDocument();
                    metadata["_id"] = model.Id;
                    metadata["title"] = model.Title;
                    metadata["body"] = model.Body;
                    metadata["filename"] = model.Filename;
                    metadata["type"] = model.Type;
                    metadata["uploaded"] = existing.Metadata["uploaded"].AsDateTime;
                    metadata["updated"] = DateTime.Now;
                    metadata["deleted"] = model.Deleted;
                    fs.SetMetadata(model.Id, metadata);

                    return true;
                }
                else
                {
                    // Base64 -> Stream
                    var bytes = Convert.FromBase64String(contentBase64);
                    var contents = new MemoryStream(bytes);

                    var metadata = new BsonDocument();
                    metadata["_id"] = model.Id;
                    metadata["title"] = model.Title;
                    metadata["body"] = model.Body;
                    metadata["filename"] = model.Filename;
                    metadata["type"] = model.Type;
                    metadata["uploaded"] = existing.Metadata["uploaded"].AsDateTime;
                    metadata["updated"] = DateTime.Now;
                    metadata["deleted"] = model.Deleted;
                    fs.Upload(model.Id, model.Filename, contents, metadata);

                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
