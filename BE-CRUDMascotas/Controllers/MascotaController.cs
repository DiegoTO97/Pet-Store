﻿using AutoMapper;
using BE_CRUDMascotas.Models;
using BE_CRUDMascotas.Models.DTO;
using BE_CRUDMascotas.Models.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_CRUDMascotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IMascotaRepository _mascotaRepository;

        public MascotaController(IMapper mapper, IMascotaRepository mascotaRepository)
        {
            _mapper = mapper;
            _mascotaRepository = mascotaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listMascotas = await _mascotaRepository.GetListMascotas();

                var listMascotasDTO = _mapper.Map<IEnumerable<MascotaDTO>>(listMascotas);

                return Ok(listMascotas);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var mascota = await _mascotaRepository.GetMascota(id);

                if (mascota == null)
                {
                    return NotFound();
                }

                var mascotaDTO = _mapper.Map<MascotaDTO>(mascota);

                return Ok(mascotaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var mascota = await _mascotaRepository.GetMascota(id);

                if (mascota == null)
                {
                    return NotFound();
                }

                await _mascotaRepository.DeleteMascota(mascota);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(MascotaDTO mascotaDTO)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDTO);
                mascota.FechaCreacion = DateTime.Now;

                mascota = await _mascotaRepository.AddMascota(mascota);

                mascotaDTO = _mapper.Map<MascotaDTO>(mascota);

                return CreatedAtAction("Get", new { id = mascotaDTO.Id }, mascotaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, MascotaDTO mascotaDTO)
        {
            try
            {
                var mascota = _mapper.Map<Mascota>(mascotaDTO);

                if(id != mascota.Id)
                {
                    return BadRequest();
                }
                
                var mascotaItem = _mascotaRepository.GetMascota(id);
               
                if ( mascotaItem == null )
                {
                    return NotFound();
                }


                await _mascotaRepository.UpdateMascota(mascota);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
