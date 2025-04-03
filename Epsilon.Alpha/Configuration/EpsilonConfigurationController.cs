using Epsilon.Alpha.Synchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Epsilon.Alpha.Configuration
{
    internal class EpsilonConfigurationController : IConfigurationController<EpsilonConfig>
    {
        private readonly string _configPath;
        private readonly JsonSerializerOptions _opts;
        private AsyncLock _lock;

        public EpsilonConfigurationController(string configPath)
        {
            _configPath = configPath;
            _opts = new JsonSerializerOptions() { WriteIndented = true, Converters = { new JsonStringEnumConverter<Keys>() } };
            _lock = new AsyncLock();
        }

        public async Task<EpsilonConfig> ReadAsync()
        {
            using (await _lock.LockAsync())
            {
                EpsilonConfig? ret = null;

                if (!string.IsNullOrWhiteSpace(_configPath) && File.Exists(_configPath))
                {
                    try
                    {
                        await using (FileStream fs = File.OpenRead(_configPath))
                            ret = await JsonSerializer.DeserializeAsync<EpsilonConfig>(fs, _opts);
                    }
                    catch
                    { }
                }

                return ret == null ? CreateDefaultConfig() : ret;
            }
        }

        public async Task WriteAsync(EpsilonConfig config)
        {
            using (await _lock.LockAsync())
            {
                if (!string.IsNullOrWhiteSpace(_configPath) && config != null)
                {
                    try
                    {
                        await using (FileStream fs = File.Create(_configPath))
                            await JsonSerializer.SerializeAsync(fs, config, _opts);
                    }
                    catch
                    { }
                }
            }
        }

        private EpsilonConfig CreateDefaultConfig()
        {
            EpsilonConfig ret = new EpsilonConfig()
            {
                ElswordTitle = "[x64] Elsword",
                BuffLocationX = 24,
                BuffLocationY = 21.0185,
                TitleSwapKey = Keys.RControlKey,
                NightParadeArrowKey = Keys.Up,
                FreedShadowArrowKey = Keys.Left,
                TheSettingSunArrowKey = Keys.Right,
                ResetTranscendenceKey = Keys.NumPad4
            };

            ret.SpecialActiveSkillKeys.Add(Keys.Q);
            ret.SpecialActiveSkillKeys.Add(Keys.W);
            ret.SpecialActiveSkillKeys.Add(Keys.E);
            ret.SpecialActiveSkillKeys.Add(Keys.T);
            ret.SpecialActiveSkillKeys.Add(Keys.S);
            ret.SpecialActiveSkillKeys.Add(Keys.C);
            ret.SpecialActiveSkillKeys.Add(Keys.F);

            ret.AwakeningKeys.Add(Keys.LControlKey);
            ret.AwakeningKeys.Add(Keys.D6);

            ret.OtherTitleArrowKeys.Add(Keys.Down);

            return ret;
        }
    }
}
