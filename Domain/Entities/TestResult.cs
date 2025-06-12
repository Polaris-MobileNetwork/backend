using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class TestResult
    {

        public Guid Id { get; set; }
        public string? ServerTestId { get; set; }

        public long Timestamp { get; set; }

        public string TestType { get; set; } = string.Empty;

        public string? TargetHost { get; set; }

        public string ResultValue { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string? Details { get; set; }

        public Guid LocalTestId { get; set; }

        [ForeignKey("LocalTestId")]
        public virtual Test? Test { get; set; }
    }
} 