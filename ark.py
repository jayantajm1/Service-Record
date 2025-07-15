import argparse
import os
import re

# --- Color Print Functions ---
def prRed(skk): print("\033[91m{}\033[00m".format(skk))
def prGreen(skk): print("\033[92m{}\033[00m".format(skk))
def prYellow(skk): print("\033[93m{}\033[00m".format(skk))
def prCyan(skk): print("\033[96m{}\033[00m".format(skk))

prCyan(".NET helper Developed By Jayanta Mardi")

# --- Code Generation Functions ---

def getRepoInterfaceCodeAndName(entity):
    ic = f"""using Service_Record.DAL.Entities;
namespace Service_Record.DAL.Interfaces
{{
    public interface I{entity}Repo: IRepository<{entity}>
    {{
    }}
}}"""
    name = f"I{entity}Repo.cs"
    return [name, ic]

def getRepoCodeAndName(entity):
    # =========================================================================ServiceRecordDbContext
    # SOLUTION: Added 'using Service_Record.DAL.Context;' to find the DbContext.
    # =========================================================================
    rc = f"""using Service_Record.DAL.Entities;
using Service_Record.DAL.Interfaces;
using Service_Record.DAL.Context;

namespace Service_Record.DAL.Repositories
{{
   public class {entity}Repo : Repository<{entity}, ServiceRecordDbContext>, I{entity}Repo
   {{
       public {entity}Repo(ServiceRecordDbContext context) : base(context)
       {{
       }}
   }}
}}"""
    name = f"{entity}Repo.cs"
    return [name, rc]

def getServcInterfaceCodeAndName(entity):
    ic = f"""using Service_Record.Models;

namespace Service_Record.BAL.Interfaces
{{
    public interface I{entity}Service
    {{
        Task<IEnumerable<{entity}Model>> GetAll{entity}();
    }}
}}"""
    name = f"I{entity}Service.cs"
    return [name, ic]

def getServcCodeAndName(entity):
    entity_lower = entity[0].lower() + entity[1:]
    sc = f"""using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Interfaces;
using Service_Record.Models;
using Service_Record.DAL.Entities;

namespace Service_Record.BAL.Services
{{
    public class {entity}Service : I{entity}Service
    {{
        private readonly I{entity}Repo _{entity_lower}Repo;
        private readonly IMapper _mapper;

        public {entity}Service(I{entity}Repo {entity_lower}Repo, IMapper mapper) {{
            _{entity_lower}Repo = {entity_lower}Repo;
            _mapper = mapper;
        }}

        public async Task<IEnumerable<{entity}Model>> GetAll{entity}()
        {{
            var allData = await _{entity_lower}Repo.GetAllAsync();
            return _mapper.Map<IEnumerable<{entity}Model>>(allData);
        }}
    }}
}}"""
    name = f"{entity}Service.cs"
    return [name, sc]

def create_file(path, CnN):
    if not os.path.exists(path):
        os.makedirs(path)
    with open(os.path.join(path, CnN[0]), "w") as f:
        f.write(CnN[1])
    prGreen(f"[+] {os.path.join(path, CnN[0])} successfully created")

def generate_model_from_entity(entity_name, entity_path, model_path):
    entity_file_path = os.path.join(entity_path, f"{entity_name}.cs")
    model_file_path = os.path.join(model_path, f"{entity_name}Model.cs")

    if not os.path.exists(entity_file_path):
        prRed(f"[-] Entity file not found at: {entity_file_path}")
        return

    with open(entity_file_path, "r") as f:
        lines = f.readlines()

    properties = []
    prop_regex = re.compile(r"^\s*public\s+[\w\<\>\[\]\?]+\s+([a-zA-Z0-9_]+)\s*\{\s*get;\s*set;")

    for line in lines:
        match = prop_regex.match(line)
        if match:
            properties.append(line.strip())

    if not properties:
        prRed(f"[-] No get/set properties found in {entity_file_path}")
        return

    model_code = "namespace Service_Record.Models\n{\n"
    model_code += f"    public class {entity_name}Model\n    {{\n"
    for prop in properties:
        model_code += f"        {prop}\n"
    model_code += "    }\n}\n"

    if not os.path.exists(model_path):
        os.makedirs(model_path)
        
    with open(model_file_path, "w") as f:
        f.write(model_code)
    prGreen(f"[+] {model_file_path} successfully created from entity.")

# --- Main Execution ---

if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        prog='ark.py',
        description='[+] ::: .NET helper By ARK:::'
    )
    parser.add_argument("entity", help="The name of the entity class (e.g., User).")
    parser.add_argument("-r", "--repo", action="store_true", help="Generate Repository and its Interface.")
    parser.add_argument("-s", "--service", action="store_true", help="Generate Service and its Interface.")
    parser.add_argument("-p", "--program", action="store_true", help="Inject dependencies into Program.cs.")
    parser.add_argument("-m", "--model", action="store_true", help="Generate a Model/DTO from an existing Entity.")
    args = parser.parse_args()

    # Define paths based on your single-project structure
    repo_interface_path = "DAL/Interfaces"
    repo_path = "DAL/Repositories"
    service_interface_path = "BAL/Interfaces"
    service_path = "BAL/Services"
    model_path = "Models"
    entity_path = "DAL/Entities" # This path is for reading entities to create models
    program_file = "Program.cs"
    
    entity_name = args.entity.capitalize()
    print('[+] Entity: ', end='')
    prYellow(f"{entity_name}")

    if args.repo:
        repoICnN = getRepoInterfaceCodeAndName(entity_name)
        repoCnN = getRepoCodeAndName(entity_name)
        create_file(repo_interface_path, repoICnN)
        create_file(repo_path, repoCnN)

    if args.service:
        servcICnN = getServcInterfaceCodeAndName(entity_name)
        servcCnN = getServcCodeAndName(entity_name)
        create_file(service_interface_path, servcICnN)
        create_file(service_path, servcCnN)

    if args.model:
        generate_model_from_entity(entity_name, entity_path, model_path)

    if args.program:
        if not os.path.exists(program_file):
            prRed(f"[-] {program_file} not found!")
        else:
            with open(program_file, "r") as f:
                content = f.read()

            if args.repo:
                repo_di_line = f"builder.Services.AddScoped<I{entity_name}Repo, {entity_name}Repo>();"
                if repo_di_line not in content:
                    content = content.replace("//Repositories", f"//Repositories\n{repo_di_line}")

            if args.service:
                service_di_line = f"builder.Services.AddScoped<I{entity_name}Service, {entity_name}Service>();"
                if service_di_line not in content:
                    content = content.replace("//Services", f"//Services\n{service_di_line}")
            
            with open(program_file, "w") as f:
                f.write(content)
            prYellow("[+] Dependency injection mappings updated in Program.cs")