
namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Person> persons = new()
            {
                new Person() { Name = "a", Address = "x" },
                new Person() { Name = "a", Address = "y" },
                new Person() { Name = "b", Address = "z" },
                new Person() { Name = "c", Address = "z" },
                new Person() { Name = "c", Address = "y" },
            };

            var relatedPersons = persons.Select(p => new RelatedPerson(p)).ToList();

            foreach (var person in relatedPersons)
            {
                foreach (var person2 in relatedPersons)
                {
                    if((person.Person.Name == person2.Person.Name 
                        || person.Person.Address == person2.Person.Address) 
                        && !person.Equals(person2))
                        person.RelatedPersons.Add(person2);
                }
            }

            Console.WriteLine(FindRelationBFS(relatedPersons, relatedPersons[0], relatedPersons[1]));
        }

        public static int FindRelationBFS(List<RelatedPerson> relatedPersons, RelatedPerson p1, RelatedPerson p2)
        {
            // Base case: if the two persons are the same, the relation distance is 0
            if (p1 == p2)
                return 0;

            // Create a queue for BFS, where each item in the queue is a tuple of RelatedPerson and distance
            Queue<(RelatedPerson person, int distance)> queue = new();
            HashSet<RelatedPerson> visited = new();  // To avoid cycles and re-visiting

            // Enqueue the starting person with a distance of 0
            queue.Enqueue((p1, 0));
            visited.Add(p1);

            while (queue.Count > 0)
            {
                // Dequeue the first person and their current distance
                var (currentPerson, currentDistance) = queue.Dequeue();

                // Explore all related persons (neighbors)
                foreach (var neighbor in currentPerson.RelatedPersons)
                {
                    // If we find the target person, return the distance (currentDistance + 1)
                    if (neighbor == p2)
                        return currentDistance + 1;

                    // If this neighbor hasn't been visited yet, enqueue it with an incremented distance
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue((neighbor, currentDistance + 1));
                    }
                }
            }

            // If we exhaust the queue and don't find p2, return -1 (no relation found)
            return -1;
        }
    }

    class Person
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    class RelatedPerson
    {
        public Person Person { get; set; }
        public List<RelatedPerson> RelatedPersons { get; set; } = new List<RelatedPerson>();

        public RelatedPerson(Person person)
        {
            this.Person.Name = person.Name;
            this.Person.Address = person.Address;
        }
    }
}
