﻿using NTMiner.Core;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace NTMiner.Controllers {
    // 注意该控制器不能重命名
    public class CaptchaController : ApiControllerBase, ICaptchaController<HttpResponseMessage> {
        /// <summary>
        /// 1，验证码在内存中保留的有效时常为10分钟
        /// 2，每次请求验证码必须传入id参数且id必须唯一否则无效
        /// 3，同一个Ip短时间内只允许请求100次
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage Get(Guid id) {
            string code;
            byte[] bytes;
            if (id == Guid.Empty) {
                bytes = CreateValidateGraphic("无效");
            }
            else if (WebApiRoot.CaptchaSet.CountByIp(base.MinerIp) > 100) {
                bytes = CreateValidateGraphic("拒绝");
            }
            else {
                code = VirtualRoot.GetRandomString(4);
                if (!WebApiRoot.CaptchaSet.SetCaptcha(CaptchaData.Create(id, code, DateTime.Now, base.MinerIp))) {
                    bytes = CreateValidateGraphic("无效");
                }
                else {
                    bytes = CreateValidateGraphic(code);
                }
            }
            var respimg = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new ByteArrayContent(bytes)
            };
            respimg.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return respimg;
        }

        private static byte[] CreateValidateGraphic(string validateCode) {
            int len = validateCode.Length;
            if (len < 4) {
                len = 4;
            }
            Bitmap image = new Bitmap((int)Math.Ceiling(len * 16.0), 27);
            Graphics g = Graphics.FromImage(image);
            try {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++) {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, x2, y1, y2);
                }
                Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);

                //画图片的前景干扰线
                for (int i = 0; i < 100; i++) {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                //输出图片流
                return stream.ToArray();
            }
            finally {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
