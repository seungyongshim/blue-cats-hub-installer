using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElastiBuild.Commands;
using ElastiBuild.Extensions;
using System.IO.Compression;

namespace ElastiBuild.BullseyeTargets
{
    public class UnpackPackageTarget : BullseyeTargetBase<UnpackPackageTarget>
    {
        public static async Task RunAsync(BuildContext ctx)
        {
            var ap = ctx.GetArtifactPackage();

            var destDir = Path.Combine(ctx.InDir, Path.GetFileNameWithoutExtension(ap.FileName));

            using var zf = ZipFile.Open(Path.Combine(ctx.InDir, ap.FileName),ZipArchiveMode.Read);

            zf.ExtractToDirectory(destDir);

            await Console.Out.WriteLineAsync($"Extracted to: {destDir}");
        }
    }
}
