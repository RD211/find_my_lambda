SELECT 
[id], 
[programming_language], 
[code], 
[input_type], 
[return_type],
[upload_date],
[times_used]
from dbo.lambdas
WHERE [id] = @lambdaId
ORDER BY [upload_date]
