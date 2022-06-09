// See https://aka.ms/new-console-template for more information
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

Console.WriteLine("Hello, World!");


// The final dimensions of the gif

// Create a blank canvas for the gif
NewMethod("carport-*", 2688 / 2, 1512 / 2, "result-24.gif", 20);
NewMethod("carport-*-1200*", 2688 / 2, 1512 / 2, "result-noon.gif", 100);

static void NewMethod(string searchPattern, int width, int height, string outputFilePath, int frameDelay)
{
    var images = System.IO.Directory.GetFiles("timelapses", searchPattern).ToList();
    Console.WriteLine(images.Count + " image(s) found that match" + searchPattern);

    using (var gif = new Image<Rgba32>(width, height))
    {
        for (int i = 0; i < images.Count; i++)
        {
            Console.WriteLine($"{i:N0} {System.IO.Path.GetFileName(images[i])}");
            // Load image that will be added
            using (var image = Image.Load(images[i]))
            {
                // Resize the image to the output dimensions
                image.Mutate(ctx => ctx.Resize(width, height));

                // Set the duration of the image
                var meta = image.Frames.RootFrame.Metadata.GetGifMetadata(); // Get or create if none.
                meta.FrameDelay = frameDelay; // Set to 30/100 of a second.

                // Add the image to the gif
                gif.Frames.InsertFrame(i, image.Frames.RootFrame);
            }
        }

        Console.WriteLine("Creating gif");
        // Save an encode the gif
        using (var fileStream = new FileStream(outputFilePath, FileMode.Create))
        {
            gif.SaveAsGif(fileStream);
        }

        Console.WriteLine("Done with " + outputFilePath);
    }
}