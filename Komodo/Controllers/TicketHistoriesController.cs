﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Komodo.Data;
using Komodo.Models;
using Microsoft.AspNetCore.Authorization;

namespace Komodo.Controllers
{
    [Authorize]
    public class TicketHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketHistories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TicketHistories
                .Include(t => t.Ticket)
                .Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketHistory = await _context.TicketHistories
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketHistory == null)
            {
                return NotFound();
            }

            return View(ticketHistory);
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketHistories/Create
        public IActionResult Create()
        {
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TicketHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,Property,OldValue,NewValue,Created,UserId")] TicketHistory ticketHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketHistory.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketHistory.UserId);
            return View(ticketHistory);
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketHistory = await _context.TicketHistories.FindAsync(id);
            if (ticketHistory == null)
            {
                return NotFound();
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketHistory.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketHistory.UserId);
            return View(ticketHistory);
        }

        // POST: TicketHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,Property,OldValue,NewValue,Created,UserId")] TicketHistory ticketHistory)
        {
            if (id != ticketHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketHistoryExists(ticketHistory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Description", ticketHistory.TicketId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ticketHistory.UserId);
            return View(ticketHistory);
        }
        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: TicketHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketHistory = await _context.TicketHistories
                .Include(t => t.Ticket)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticketHistory == null)
            {
                return NotFound();
            }

            return View(ticketHistory);
        }

        // POST: TicketHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketHistory = await _context.TicketHistories.FindAsync(id);
            _context.TicketHistories.Remove(ticketHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketHistoryExists(int id)
        {
            return _context.TicketHistories.Any(e => e.Id == id);
        }
    }
}
