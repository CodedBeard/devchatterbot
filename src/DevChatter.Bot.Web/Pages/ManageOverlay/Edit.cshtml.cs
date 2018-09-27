﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Infra.Ef;

namespace DevChatter.Bot.Web.Pages.ManageOverlay
{
    public class EditModel : PageModel
    {
        private readonly DevChatter.Bot.Infra.Ef.AppDataContext _context;

        public EditModel(DevChatter.Bot.Infra.Ef.AppDataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CanvasProperties CanvasProperties { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CanvasProperties = await _context.CanvasProperties.FirstOrDefaultAsync(m => m.Id == id);

            if (CanvasProperties == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CanvasProperties).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CanvasPropertiesExists(CanvasProperties.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CanvasPropertiesExists(Guid id)
        {
            return _context.CanvasProperties.Any(e => e.Id == id);
        }
    }
}
