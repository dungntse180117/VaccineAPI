using System;
using System.Collections.Generic;
using System.Linq;
using VaccineAPI.DataAccess.Data;  // Namespace của DbContext
using Microsoft.EntityFrameworkCore;

namespace VaccineAPI.DataAccess.Models;

public class ChildService : IChildService
{
    private readonly VaccinationTrackingContext _context;

    public ChildService(VaccinationTrackingContext context)
    {
        _context = context;
    }

    public List<ChildResponse> GetAllChildren()
    {
        return _context.Children
            .Select(c => new ChildResponse
            {
                ChildId = c.ChildId,
                Dob = c.Dob,
                ChildName = c.ChildName,
                Gender = c.Gender,
                VaccinationStatus = c.VaccinationStatus
            })
            .ToList();
    }

    public ChildResponse GetChildById(int id)
    {
        return _context.Children
            .Where(c => c.ChildId == id)
            .Select(c => new ChildResponse
            {
                ChildId = c.ChildId,
                Dob = c.Dob,
                ChildName = c.ChildName,
                Gender = c.Gender,
                VaccinationStatus = c.VaccinationStatus
            })
            .FirstOrDefault();
    }

    public ChildResponse CreateChild(ChildRequest childRequest)
    {
        var child = new Child
        {
            Dob = childRequest.Dob,
            ChildName = childRequest.ChildName,
            Gender = childRequest.Gender,
            VaccinationStatus = childRequest.VaccinationStatus
        };

        _context.Children.Add(child);
        _context.SaveChanges();

        return new ChildResponse
        {
            ChildId = child.ChildId,
            Dob = child.Dob,
            ChildName = child.ChildName,
            Gender = child.Gender,
            VaccinationStatus = child.VaccinationStatus
        };
    }

    public ChildResponse UpdateChild(int id, ChildRequest childRequest)
    {
        var existingChild = _context.Children.Find(id);
        if (existingChild == null)
        {
            return null; // Hoặc ném một exception
        }

        existingChild.Dob = childRequest.Dob;
        existingChild.ChildName = childRequest.ChildName;
        existingChild.Gender = childRequest.Gender;
        existingChild.VaccinationStatus = childRequest.VaccinationStatus;

        // _context.Children.Update(existingChild);  // Cách này cũng hoạt động
        _context.Entry(existingChild).State = EntityState.Modified; // Đảm bảo EF Core theo dõi thay đổi
        _context.SaveChanges();

        return new ChildResponse
        {
            ChildId = existingChild.ChildId,
            Dob = existingChild.Dob,
            ChildName = existingChild.ChildName,
            Gender = existingChild.Gender,
            VaccinationStatus = existingChild.VaccinationStatus
        };
    }

    public void DeleteChild(int id)
    {
        var child = _context.Children.Find(id);
        if (child != null)
        {
            _context.Children.Remove(child);
            _context.SaveChanges();
        }
    }
}