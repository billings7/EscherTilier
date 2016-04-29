using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EscherTiler.Graphics;
using EscherTiler.Storage;
using JetBrains.Annotations;

namespace EscherTiler.Templates
{
    /// <summary>
    ///     Manages the built in templates.
    /// </summary>
    public static class TemplateManager
    {
        [NotNull]
        private static readonly List<Tuple<Template, IImage>> _templates = new List<Tuple<Template, IImage>>();

        /// <summary>
        ///     Initializes the <see cref="TemplateManager" /> class.
        /// </summary>
        static TemplateManager()
        {
            foreach (string templatePath in Directory.GetFiles("Templates", "*.eschtmp"))
            {
                Debug.Assert(templatePath != null, "templatePath != null");

                try
                {
                    IImage thumbnail;
                    Template template = FileStorage.LoadTemplate(templatePath, out thumbnail);

                    _templates.Add(Tuple.Create(template, thumbnail));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"{e.GetType().Name} - {e.Message}");
#if DEBUG
                    throw;
#endif
                }
            }
        }

        /// <summary>
        ///     Gets the templates.
        /// </summary>
        /// <value>
        ///     The templates.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<Tuple<Template, IImage>> Templates => _templates;
    }
}