﻿using AutoMapper;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Vidly.DTOs;
using Vidly.Models;

namespace Vidly.Controllers.API
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // Building CRUD APIs

        // GET  /api/customers
        public IHttpActionResult GetCustomers()
        {
            return Ok(_context.Customers.
                Include(c=>c.MemberShipType).
                ToList().
                Select(Mapper.Map<Customer, CustomerDto>));
        }

        //GET   /api/customers/1

        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.Include(c=>c.MemberShipType).SingleOrDefault(c => c.Id == id);

            if (customer == null)
                NotFound();

            return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }


        // POST  request will be through the URL :  api/customers
        [HttpPost]
        public IHttpActionResult AddCustomers(CustomerDto customerDto) // it will return the resource added which is customerDto
        {
            // first check the modal state , which is sent from the Request
            if (!ModelState.IsValid)
                return BadRequest();

            var addedCustomer = Mapper.Map<CustomerDto, Customer>(customerDto);

            _context.Customers.Add(addedCustomer);      // saved in Memory, need to save in DB
            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + addedCustomer.Id), addedCustomer);
        }

        // PUT to fully update the object with full properties unlike PATCH  api/customers/1
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDto customerDto) // object properties in request body
        {
            // CHeck the Model state which is sent from the Request
            if (!ModelState.IsValid) return BadRequest();
            // first i need to catch that customerDto with id from DB 
            var customerDB = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerDB == null)
                NotFound();

            // now we need to MAP  (you can use AutoMapper)
            var updatedCustomer = Mapper.Map<CustomerDto, Customer>(customerDto, customerDB);

            _context.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + updatedCustomer.Id), updatedCustomer);

        }

        //DELETE  request will be through   api/customers/1
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customerDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            _context.Customers.Remove(customerDb);
            _context.SaveChanges();
        }


    }
}
