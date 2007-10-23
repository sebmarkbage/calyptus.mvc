using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calyptus.MVC
{
    public class Root
    {
        public Root()
        {
            this.DB = new DBContext();
        }

        [Action]
        public Users Users()
        {
            return new Users(this.DB.Users);
        }

        [Action]
        public Blogs Blogs()
        {
            return new Blogs(this.DB.Blogs);
        }
    }


    public class UsersController
    {
        public UsersController(IQueryable<User> users)
        {

        }

        public UsersController User(string friendlyid)
        {
            return new UsersController(users.Where(u => u.FriendlyID == friendlyid).First());
        }

        public Blogs Blogs()
        {
            return new Blogs(users.Where(friendlyid).First().Blogs());
        }
    }

    public class UserController
    {
        public UserController(User user)
        {

        }
    }

    public class Blogs
    {
        public Blogs(IQueryable<Blog> blogs)
        {

        }
    }

    public class Galleries
    {
        public Galleries([Parent] Users usersController)
        {

        }
    }


    public class Posts
    {
        public Posts(IQueryable<Post> posts)
        {

        }
    }

    //  URL(() => new Root().Users().SelectUser("Seb").Blogs());
}
