using ItineraryOperations.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ItineraryOperations.Models.Executor
{
    public class ExecutorDto
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public bool IsBrigade { get; set; }
        [Required]
        public required string[] Members { get; set; } = Array.Empty<string>();

        [Required]
        public int DivisionID { get; set; }

        [Required]
        public int[] OperationsIDs { get; set; } = Array.Empty<int>();

        [SetsRequiredMembers]
        public ExecutorDto(Executors executor)
        {
            ID = executor.ID;
            IsBrigade = executor.isBrigade;
            Members = executor.Members;
            DivisionID = executor.DivisionID;
            OperationsIDs = [];
        }

        public ExecutorDto()
        {
        }
    }
}