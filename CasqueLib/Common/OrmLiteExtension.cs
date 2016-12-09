using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ServiceStack.OrmLite;

namespace CasqueLib.Common
{
  /// <summary>
  /// Classe pour l'extension du langage ORM LITE
  /// Prise en compte des procédures qui renvoient plusieurs tables.
  /// </summary>
  public static class OrmLiteExtension
  {
    /// <summary>
    /// Renvoie la liste typée à partir d'un reader
    /// </summary>
    /// <typeparam name="T">Le type</typeparam>
    /// <param name="dataReader">Le reader</param>
    /// <returns>La liste des éléménts</returns>
    public static List<T> CustomConvertToList<T>(this IDataReader dataReader)
    {
      var modelDef = ModelDefinition<T>.Definition;
      var type = typeof(T);
      var fieldDefs = modelDef.AllFieldDefinitionsArray;
      var listInstance = Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
      var to = (IList)listInstance;
      var indexCache = dataReader.GetIndexFieldsCache(modelDef);

      while (dataReader.Read())
      {
        var row = Activator.CreateInstance(type);
        row.PopulateWithSqlReader(dataReader, fieldDefs, indexCache);
        to.Add(row);
      }

      return to.Cast<T>().ToList();
    }

    /// <summary>
    /// renvoie le dico des infos du reader
    /// </summary>
    /// <param name="reader">Le reader</param>
    /// <param name="modelDefinition">Le modèle de définition</param>
    /// <returns>Le dictionnaire</returns>
    private static Dictionary<string, int> GetIndexFieldsCache(this IDataReader reader, ModelDefinition modelDefinition = null)
    {
      var cache = new Dictionary<string, int>();
      if (modelDefinition != null)
      {
        foreach (var field in modelDefinition.IgnoredFieldDefinitions)
        {
          cache[field.FieldName] = -1;
        }
      }

      for (var i = 0; i < reader.FieldCount; i++)
      {
        cache[reader.GetName(i)] = i;
      }

      return cache;
    }
  }
}
