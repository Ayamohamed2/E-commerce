using E_commerce.Data;
using E_commerce.Dataaccess.Reposatory.IReposatory;
using E_commerce.Model.Models;
using E_commerce_project.Migrations;
using E_commerce_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Dataaccess.Reposatory
{
    public class FeedbackReposatory:Reposatory<Feedback>,IFeedbackReposatory
    {
        Context Context;
        public FeedbackReposatory(Context context):base(context)
        {
            this.Context = context;
        }


      
    }
}
