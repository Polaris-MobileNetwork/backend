using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    
    public class Test
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string ParametersJson { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

        public long? ScheduledTimestamp { get; set; }

        public int? IntervalSeconds { get; set; }

        public bool IsCompleted { get; set; } = false;

        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
} 