using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElastiBuild.Commands;
using ElastiBuild.Extensions;
using Ionic.Zip;

namespace ElastiBuild.BullseyeTargets
{
    public class UnpackPackageTarget : BullseyeTargetBase<UnpackPackageTarget>
    {
        public static async Task RunAsync(BuildContext ctx)
        {
            var ap = ctx.GetArtifactPackage();

            var destDir = Path.Combine(ctx.InDir, Path.GetFileNameWithoutExtension(ap.FileName));

            using var zf = ZipFile.Read(Path.Combine(ctx.InDir, ap.FileName));

            Directory.CreateDirectory(destDir);

            int totalItems = zf.Count;
            int currentItem = 0;

            foreach (var itm in zf.Entries)
            {
                var fname = itm.FileName;
                if (itm.IsDirectory)
                {
                    Directory.CreateDirectory(
                        Path.Combine(destDir, fname));
                }
            }

            foreach (var itm in zf.Entries)
            {
                var fname = itm.FileName;

                if (itm.IsDirectory)
                {
                    Directory.CreateDirectory(
                        Path.Combine(destDir, fname));
                }
                else
                {
                    using var fs = File.Open(
                        Path.Combine(destDir, fname),
                        FileMode.Create,
                        FileAccess.Write);

                    itm.Extract(fs);
                }

                double progress = ((++currentItem * 100.0) / totalItems);
                if (progress % 10 == 0)
                    Console.WriteLine((int) progress + "%");
            }

            await Console.Out.WriteLineAsync($"Extracted to: {destDir}");
        }
    }
}
