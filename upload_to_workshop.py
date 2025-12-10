import os
import re
import subprocess


SEWT = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\SpaceEngineers\\Bin64\\SEWorkshopTool.exe"
MODS_DIR = "SpaceEngineers\\Mods"

EXCLUDE_FILES = [".bat", ".psd", ".md", ".py", ".gitattributes", ".gitignore", ".sln", ".csproj", ".mod", ".sbmi"]
EXCLUDE_DIRS = ["bin", "obj", "WebWiki", ".git"]

UPLOAD_DESC = False
UPLOAD_PN = True

TAGS = ["Other", "ServerScripts", "NPC"]

MOD_NAME_OVERRIDE = "Modular Encounters Systems"

DRY_RUN = False

def md_to_steam_bbcode(md: str) -> str:
    bbcode = []

    open_quote = False

    open_b = False
    open_i = False

    previous_new_l = ""

    lines = md.splitlines()
    for l in lines:
        new_l = l

        # Images with links [![](IMAGE_LINK)](WEB_LINK)
        pattern = r'\[!\[\]\(([^)]+)\)\]\(([^)]+)\)'
        matches = re.findall(pattern, new_l)
        for match in matches:
            image_link, web_link = match
            new_l = new_l.replace(f"[![]({image_link})]({web_link})", f"[url={web_link}][img]{image_link}[/img][/url]")

        # Simple Images ![](IMAGE_LINK)
        pattern = r'!\[([^\]]*)\]\(([^)]+)\)'
        matches = re.findall(pattern, new_l)
        for match in matches:
            alt_text, image_link = match
            new_l = new_l.replace(f"![{alt_text}]({image_link})", f"[img]{image_link}[/img]")

        # Simple URLs [LINK_TEXT](WEB_LINK)
        pattern = r'\[([^\]]+)\]\(([^)]+)\)'
        matches = re.findall(pattern, new_l)
        for match in matches:
            link_text, web_link = match
            new_l = new_l.replace(f"[{link_text}]({web_link})", f"[url={web_link}]{link_text}[/url]")

        # Headings
        if new_l.startswith("### "):
            new_l =  new_l.replace("### ", "[h3]") + "[/h3]"
        elif new_l.startswith("## "):
            new_l = new_l.replace("## ", "[h2]") + "[/h2]"
        elif new_l.startswith("# "):
            new_l = new_l.replace("# ", "[h1]") + "[/h1]"

        # Unordered Lists
        if new_l.startswith("* "):
            new_l = new_l.replace("* ", "[*]", 1)
            if not bbcode == [] and not previous_new_l.startswith("[*]"):
                bbcode[-1] = bbcode[-1] + "[list]" + "\n"
            if lines.index(l) + 1 >= len(lines):
                new_l += "\n[/list]"
        else:
            if previous_new_l.startswith("[*]") and not new_l.startswith("* "):
                new_l = "[/list]\n" + new_l

        # Quote
        if new_l.startswith("> "):
            new_l = new_l.replace("> ", "")
            if not open_quote:
                if not bbcode == [] and not previous_new_l.startswith("[quote]"):
                    bbcode[-1] = bbcode[-1] + "[quote]" + "\n"
                    open_quote = True
            if open_quote and lines.index(l) + 1 >= len(lines):
                new_l += "\n[/quote]"
        else:
            if open_quote:
                new_l = "[/quote]\n" + new_l
                open_quote = False

        if "**" in new_l:
            while "**" in new_l:
                if open_b:
                    new_l = new_l.replace("**", "[/b]", 1)
                    open_b = False
                else:
                    new_l = new_l.replace("**", "[b]", 1)
                    open_b = True

        if "*" in new_l:
            while "*" in new_l and not (new_l.startswith("[*]") and new_l.count("*") <= 1):
                if open_i:
                    new_l = new_l.replace("*", "[/i]", 1)
                    open_i = False
                else:
                    new_l = new_l.replace("*", "[i]", 1)
                    open_i = True

        if previous_new_l.endswith("[/h1]\n") or previous_new_l.endswith("[/h2]\n") or previous_new_l.endswith("[/h3]\n") or new_l.endswith("[/img]"):
            pass
        else:
            new_l += "\n"

        bbcode.append(new_l)
        previous_new_l = new_l

    bbcode_text = ""
    for item in bbcode:
        bbcode_text += item

    return bbcode_text


def path_adjustments() -> str:
    new_path = ""
    current_path = os.path.dirname(__file__)

    if current_path.endswith("\\Content"):
        mod_name = current_path.split("\\")[-2]
    elif current_path.endswith("\\modpack"):
        mod_name = current_path.split("\\")[-2]
    else:
        mod_name = current_path.split("\\")[-1]

    if MOD_NAME_OVERRIDE != "":
        new_path = os.path.join(os.getenv('APPDATA'), MODS_DIR, MOD_NAME_OVERRIDE)
    else:
        new_path = os.path.join(os.getenv('APPDATA'), MODS_DIR, mod_name)

    return new_path

def main():

    mod_dir = path_adjustments()
    os.chdir(mod_dir)

    if DRY_RUN:
        args = [SEWT, "push", "--dry-run", "--compile", "--mods", mod_dir]
    else:
        args = [SEWT, "push", "--compile", "--mods", mod_dir]

    if len(EXCLUDE_FILES) > 0:
        args.append("--exclude-ext")
        for i in EXCLUDE_FILES:
            args.append(i)

    if len(EXCLUDE_DIRS) > 0:
        args.append("--exclude-path")
        for i in EXCLUDE_DIRS:
            args.append(i)


    if UPLOAD_DESC:
        desc_src_path = os.path.join(mod_dir, "description.md")

        if os.path.exists(desc_src_path):
            args.append("--description")
            args.append("description_tmp.md")

            with open(desc_src_path, 'r') as f:
                description_text = f.read()
            f.close()

            desc_path = os.path.join(mod_dir, "description_tmp.md")
            desc_file = open(desc_path, "w")
            desc_file.write(md_to_steam_bbcode(description_text))
            desc_file.close()

    if UPLOAD_PN:
        pn_src_path = os.path.join(mod_dir, "patch_notes.md")

        if os.path.exists(pn_src_path):
            args.append("--message")
            args.append("patch_notes_tmp.md")

            with open(pn_src_path, 'r') as f:
                patch_notes_text = f.read()
            f.close()

            pn_path = os.path.join(mod_dir, "patch_notes_tmp.md")
            pn_file = open(pn_path, "w")
            pn_file.write(md_to_steam_bbcode(patch_notes_text))
            pn_file.close()

    if len(TAGS) > 0:
        args.append("--tags")
        for tag in TAGS:
            args.append(tag)

    subprocess.call(args, cwd=mod_dir, shell=True)

    if UPLOAD_DESC and os.path.exists(desc_path):
        os.remove(desc_path)
    if UPLOAD_PN and os.path.exists(pn_path):
        os.remove(pn_path)

    return


if __name__ == '__main__':
    main()