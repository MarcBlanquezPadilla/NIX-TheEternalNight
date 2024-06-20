$scriptPath = $PSScriptRoot

$exportFilePath = $scriptPath+"\Texts.xlsm"
$macro = 'ExportCSVMacro'

$excel = New-Object -ComObject Excel.Application
$workBook = $excel.Workbooks.Open($exportFilePath, 0, 0, 5, 0)
$workSheet = $workBook.Worksheets.Item(1)

$excel.Run($macro)
$workBook.Close()
$excel.Quit()

$convertFilePath = $scriptPath +"\..\Assets\Texts\Texts.csv"
$content = get-content $convertFilePath
    out-file -filepath $convertFilePath -inputobject $content -encoding utf8