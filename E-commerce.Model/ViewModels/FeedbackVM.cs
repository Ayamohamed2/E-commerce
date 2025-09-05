using E_commerce.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Model.ViewModels
{
    public class FeedbackVM
    {

        public Feedback NewFeedback { get; set; }

        public Product product { get; set; }

        public List<Feedback> Feedbacks { get; set; }
        public double AverageRating { get; set; }
    }
}
