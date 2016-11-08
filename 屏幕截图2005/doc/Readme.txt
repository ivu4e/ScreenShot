一个Gif图像文件，是有几个文件进行合成的，因此处理此类文件的时候，不能像Jpeg或者Bmp文件那样处理。
需要把Gif文件拆分帧的形式，然后对每一帧进行处理，处理完后再合成Gif。
  
AnimatedGifEncoder 类的实现已封装在 Gif.Components.dll 中,
工程文件中要引入Gif.Components.dll, 并引用如下:
using System; 
using System.Drawing; 
using System.Drawing.Imaging; 
using Gif.Components; // AnimatedGifEncoder 类的命名空间.
  

    
// 创建 Gif 
            String [] imageFilePaths = new String[]{"c:\\01.png","c:\\02.png","c:\\03.png"};    
            String outputFilePath = "c:\\test.gif";   
            AnimatedGifEncoder e = new AnimatedGifEncoder();   
            e.Start( outputFilePath );   
            e.SetDelay(500);    // 延迟间隔
            e.SetRepeat(0);  //-1:不循环,0:总是循环 播放   
            for (int i = 0, count = imageFilePaths.Length; i < count; i++ )   
            {  
                e.AddFrame( Image.FromFile( imageFilePaths[i] ) );  
            }  
            e.Finish();  
            
            

// 提取 Gif   
            string outputPath = "c:\\";  
            GifDecoder gifDecoder = new GifDecoder();  
            gifDecoder.Read( "c:\\test.gif" );  1
            for ( int i = 0, count = gifDecoder.GetFrameCount(); i < count; i++ )   
            {  
                Image frame = gifDecoder.GetFrame( i ); // frame i  
                frame.Save( outputPath + Guid.NewGuid().ToString() + ".png", ImageFormat.Png );  
            }

附上三个PNG图片,放到C盘根目录即可.