"""Execute read/error probes from requests.rest against the isolated Testing server."""
from pathlib import Path
import re
import urllib.request
import urllib.error

root=Path(__file__).parent
source=(root/'requests.rest').read_text(encoding='utf-8-sig')
base=re.search(r'@baseUrl\s*=\s*(\S+)',source).group(1)
lines=[]
for block in source.split('###')[1:]:
    if 'Зупиніть' in block: continue
    match=re.search(r'(?m)^(GET|POST|DELETE) \{\{baseUrl\}\}([^\r\n]+)',block)
    if not match: continue
    method,path=match.groups()
    headers=dict(re.findall(r'(?m)^(Content-Type|X-PR46-Fault):\s*([^\r\n]+)',block))
    body=re.search(r'(?m)^\{[^\r\n]*\}',block)
    request=urllib.request.Request(base+path, data=body.group(0).encode() if body else None, headers=headers, method=method)
    try:
        response=urllib.request.urlopen(request,timeout=5)
    except urllib.error.HTTPError as error:
        response=error
    with response:
        payload=response.read().decode('utf-8')
        lines.append(f'{method} {path}: HTTP {response.status}\n{payload}\n')
output='\n'.join(lines)
(root/'verification'/'rest-results.txt').write_text(output,encoding='utf-8')
print('\n'.join(line.split('\n')[0] for line in lines))
